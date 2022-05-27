# coding=utf-8
#!/usr/bin/env python
## Functions for processing data from XRF.


## SPECIAL CONFIGURATION START
## In production mode, working directory and path of library will change
IN_PRODUCTION = True
import os, sys
if IN_PRODUCTION is True:
    os.chdir(os.path.join(os.getcwd(), "Python"))
    sys.path = [
        '',
        os.path.join(os.getcwd(), "Scripts"),
        os.path.join(os.getcwd(), "python39"),
        os.path.join(os.getcwd(), "lib\\site-packages")
    ]
## SPECIAL CONFIGURATION END


import math
import json
import numpy as np
from shlex import split
from scipy import integrate
from scipy import sparse
from scipy.sparse.linalg import spsolve
from argparse import ArgumentParser
from argparse import ArgumentError
from scipy.signal import savgol_filter
from scipy import stats
from scipy.ndimage import gaussian_filter1d
from scipy.signal import find_peaks
from sympy import symbols, nsolve


## Save location of this file into path, app using portable python with different location as script.
file_dir = os.path.dirname(__file__)
sys.path.append(file_dir)


## After path is updated path of scripts_helper will be recognized.
import scripts_helper


## Script information.
__author__ = "Bc. Frantisek Pastorek"
__copyright__ = "Copyright 2022, Software design for X-ray fluorescence spectrometry"
__credits__ = ["Bc. Frantisek Pastorek", "Ing. Katarína Sedlačková, PhD.", ""]
__license__ = "GPL"
__version__ = "0.1.0"
__maintainer__ = "Bc. Frantisek Pastorek"
__email__ = "[xpastorekf@stuba.sk, frantisekpastorek@gmail.com]"
__status__ = "Development"


## Calculating net area directly by sum of trapezoids under plotpip install 
def net_area_simple(x, y):
    x = x.tolist()
    y = y.tolist()
    net_area = 0
    for index in range(len(x) - 1):
        a = y[index]
        c = y[index + 1]
        d = x[index + 1] - x[index]
        net_area += (a + c) * d / 2
    return net_area


def get_mass_absorption_coeficients(data, primary_element, energies):
    element_xmc = data[primary_element - 1]
    predicted_values = []
    for energy in energies:
        left_base_index, right_base_index = get_border_indexes(element_xmc["energy"], energy)
        left_base_energy = element_xmc["energy"][left_base_index]
        right_base_energy = element_xmc["energy"][right_base_index]
        left_base_absorption = element_xmc["mass_absorption_coefficient"][left_base_index]
        right_base_absorption = element_xmc["mass_absorption_coefficient"][right_base_index]
        regression = scripts_helper.linear_regression([left_base_energy, right_base_energy], [left_base_absorption, right_base_absorption])
        predicted_values.append(scripts_helper.predict(regression, [energy])[0])
    return predicted_values


def get_border_indexes(inputArray, inputValue):
    for index in range(len(inputArray)):
        if inputArray[index] - inputValue >= 0:
            return index - 1, index


def get_mass_attenuation_coeficients(data, primary_element, energies):
    element_xmc = data[primary_element - 1]
    predicted_values = []
    for energy in energies:
        left_base_index, right_base_index = get_border_indexes(element_xmc["energy"], energy)
        left_base_energy = element_xmc["energy"][left_base_index]
        right_base_energy = element_xmc["energy"][right_base_index]
        left_base_attenuation = element_xmc["mass_attenuation_coefficient"][left_base_index]
        right_base_attenuation = element_xmc["mass_attenuation_coefficient"][right_base_index]
        regression = scripts_helper.linear_regression([left_base_energy, right_base_energy], [left_base_attenuation, right_base_attenuation])
        predicted_values.append(scripts_helper.predict(regression, [energy])[0])
    return predicted_values
            

def get_jump_ratios(data, elements, elements_lines):
    result = []
    for element_index in range(len(elements)):
        for jump_ratio in data[elements[element_index - 1]]["jump_ratios"]:
            if elements_lines[element_index].startswith(jump_ratio["line"]):
                result.append(jump_ratio["jump_ratio"])
                break
    return result


def get_fluorescent_yields(data, elements, elements_lines):
    result = []
    for element_index in range(len(elements)):
        for probability in data[elements[element_index] - 1]["probabilities"]:
            if elements_lines[element_index].startswith(probability["line"]):
                result.append(probability["probability"])
                break
    return result


## Ocakava jeden element zo suboru "detector_efficiency.json"
## Teda ako vstup uz vybrany detector
def get_efficiency(data, energies):
    result = []
    for energy in energies:
        closest_index = None
        closest_value = float('inf')
        dee = data["energy"]
        for index in range(len(dee)):
            value = abs(energy - dee[index])
            if value < closest_value:
                closest_value = value
                closest_index = index
        result.append(data["total_attenuation"][closest_index])
    return result


## Using simpson rule directly from scipy library
def net_area_simpson(x, y):
    return integrate.simpson(y = y, x = x, dx = 1, axis = -1)


def quantitative_analysis_equation(p, coeficients):
    equations = []
    for i in range(len(coeficients)):
        eq = 0
        for j in range(len(p) - 1):
            eq += coeficients[i][j] * p[j]
        eq -= coeficients[i][-1] * p[i] * p[-1]
        equations.append(eq)

    eq = 0
    for j in range(len(p) - 1):
        eq += p[j]
    eq -= 1
    equations.append(eq)

    return equations


## Writing into file. For logging output purpose.
def write_file(data, mode="a+"):
    file = open("./output.txt", mode)
    file.write(json.dumps(data, cls = scripts_helper.NumpyEncoder))
    file.write("\n")
    file.close()


#####################################
#####################################
#####################################
#####                           #####
#####   COMMUNICATION METHODS   #####
#####                           #####
#####################################
#####################################
#####################################


## Gross area
def gross_area_trapezoidal(data_x, data_y):
    areas = []
    if scripts_helper.is_array(data_x[0]) and scripts_helper.is_array(data_y[0]):
        for (x, y) in zip(data_x, data_y):
            areas.append(integrate.trapz(y = y, x = x, dx = 1, axis = -1))
    elif scripts_helper.is_array(data_x[0]) or scripts_helper.is_array(data_y[0]):
        pass
    else:
        areas.append(integrate.trapz(y = data_y, x = data_x, dx = 1, axis = -1))

    return json.dumps({"data": areas})

## Net area
def net_area_trapezoidal(data_x, data_y):
    areas = []
    if scripts_helper.is_array(data_x[0]) and scripts_helper.is_array(data_y[0]):
        for (x, y) in zip(data_x, data_y):
            areas.append(integrate.trapz(y = y, x = x, dx = 1, axis = -1) - integrate.trapz(y = [y[0], y[-1]], x = [x[0], x[-1]], dx = 1, axis = -1))
    elif scripts_helper.is_array(data_x[0]) or scripts_helper.is_array(data_y[0]):
        pass
    else:
        areas.append(integrate.trapz(y = data_y, x = data_x, dx = 1, axis = -1) - integrate.trapz(y = [data_y[0], data_y[-1]], x = [data_x[0], data_x[-1]], dx = 1, axis = -1))

    return json.dumps({"data": areas})


## Function use ordinary least squares (OLS) to calculate aproximate solution
## for overdeterminet system of linear equations
def system_of_linear_equation_OLS(A, B):
    A = np.asarray(A, dtype=np.float32)
    B = np.asarray(B, dtype=np.float32)

    #A^T
    At = A.transpose()

    #A^T.A
    mul_At_A = np.matmul(At, A)

    #(A^T.A)^-1
    inv_At_A = np.linalg.inv(mul_At_A)

    #((A^T.A)^-1).A^T
    mul_inv_At = np.matmul(inv_At_A, At)

    #(((A^T.A)^-1).A^T).B
    mul_int_At_B = np.matmul(mul_inv_At, B)
    return json.dumps({"result": mul_int_At_B}, cls = scripts_helper.NumpyEncoder)


## Load data by file name
def load_data(full_file_name):
    file = open(full_file_name, "r", encoding="utf8")
    file_content = file.read()
    file.close()
    file_content = json.loads(file_content)
    return json.dumps({"data": file_content}, cls = scripts_helper.NumpyEncoder)


## Qualitative analysis
## Best recommended match is in attribute best_matches as last result
def qualitative_analysis(elements_lines, energies, energy_abs_treshold = 0.04, rate_treshold = 0.02):
    #energy_example = [4.65, 3.68, 3.27, 2.96, 7.05, 5.95, 6.40, 5.42]
    #energy_example2 = [6.42, 7.7, 2.98, 5.44, 7.50, 5.93, 3.72, 3.22, 8.28, 1.77, 4.63, 8.7, 2.33, 12.81, 4.04, 4.97, 13.47, 1.50, 2.67, 1.23]
    energy_example2 = sorted(energies, key=lambda x: x, reverse=False)
    #print(energy_example2)
    #energy_example3 = [3.27]
    energies = energy_example2

    result = []
    for energy in energies:
        result.append({"energy":  energy, "possible_matches": [], "best_matches": []})
    
    # Get all possible matcher based on rate
    # Filter them by rate_treshold
    for element in result:
        for element_line in elements_lines:
            abs_value = abs(element_line["energy"] - element["energy"])
            if abs_value <= energy_abs_treshold and element_line["rate"] >= rate_treshold:
                line_energy_copy = element_line.copy()
                line_energy_copy["abs"] = abs_value
                element["possible_matches"].append(line_energy_copy)

    # First iteration
    # Sort possible matches by rate DESC
    # Save first from possible matches and add to the best match (will be first/last element of the list)
    for element in result:
        if len(element["possible_matches"]) > 0:
            element["possible_matches"] = sorted(element["possible_matches"], key=lambda x: x["rate"], reverse=True)
            first_possible_match = element["possible_matches"][0].copy()
            first_possible_match["iteration"] = 1
            element["best_matches"].append(first_possible_match)

    # Second iteration
    # Only for energy where last best match line not starts with K (line)
    # Check if second possible match element is equal with best match 1 element of another energy
    for element in result:
        if len(element["best_matches"]) and not element["best_matches"][-1]["line"].startswith("K"):
            for possible_match in element["possible_matches"]:
                if element["best_matches"][-1]["iteration"] == 2:
                    break
                for other_element in filter(lambda x: x["energy"] != element["energy"], result):
                    if len(other_element["best_matches"]) and possible_match["element"] == other_element["best_matches"][-1]["element"]:
                        second_possible_match = possible_match.copy()
                        second_possible_match["iteration"] = 2
                        element["best_matches"].append(second_possible_match)
                        break

    # Third iteration
    # Only for energy where last best match line not starts with K (line) or L (line)
    for element in result:
        if len(element["possible_matches"]) > 1 and not element["best_matches"][-1]["line"].startswith("K"):
            for possible_match in element["possible_matches"]:
                if possible_match["line"].startswith("K"):
                    third_possible_match = possible_match.copy()
                    third_possible_match["iteration"] = 3
                    element["best_matches"].append(third_possible_match)
                    break
            if element["best_matches"][-1]["iteration"] == 3 or element["best_matches"][-1]["line"].startswith("L"):
                continue
            for possible_match in element["possible_matches"]:
                if possible_match["line"].startswith("L"):
                    third_possible_match = possible_match.copy()
                    third_possible_match["iteration"] = 3
                    element["best_matches"].append(third_possible_match)
                    break

    return json.dumps({"data": result}, cls = scripts_helper.NumpyEncoder)


## epsilon_i    - detector efficiencies
## dN_i         - peaks intensities
## tau_1i       - mass absorption efficiencies of primary element
## omega_i      - fluorescent yields
## ptr_i        - transitions probabilities (rad rates of elements)
## r_i          - edges jump ratios
## phi          - xray tube angel
## psi          - detector angel
## mu_1j        - mass attenuation coeficients of primary element
## mu_ij        - mass attenuation coeficients of elements between each other
def quantitative_analysis(xray_mass_coefficients, detector_efficiencies, fluorescent_yields, jump_ratios, primary_element, elements, elements_energies, elements_areas, elements_radrates, elements_lines, x_ray_tube_angel, detector_angel):
    epsilon_i = get_efficiency(detector_efficiencies, elements_energies)
    dN_i = elements_areas
    tau_1i = get_mass_attenuation_coeficients(xray_mass_coefficients, primary_element, elements_energies)
    omega_i = get_fluorescent_yields(fluorescent_yields, elements, elements_lines)
    ptr_i = elements_radrates
    r_i = get_jump_ratios(jump_ratios, elements, elements_lines) # hodilo by sa mat presnejsie hodnoty
    phi = math.radians(x_ray_tube_angel)
    psi = math.radians(detector_angel)
    mu_1j = get_mass_absorption_coeficients(xray_mass_coefficients, primary_element, elements_energies)
    mu_ij = [get_mass_absorption_coeficients(xray_mass_coefficients, element, elements_energies) for element in elements]

    number_of_elements = len(dN_i)
    concentrations_equations = []
    for i in range(number_of_elements):
        # calculate coeficients
        c1 = dN_i[i]
        c2 = epsilon_i[i] * tau_1i[i] * omega_i[i] * ptr_i[i] * (1 - 1/r_i[i])

        # sum all absorption coeficients which i interacts
        mu_1j_equation = []
        for j in range(number_of_elements):
            mu_1j_equation.append(mu_1j[j] / math.cos(phi))

        # sum all absorption coeficients which elements interracts between each other
        mu_ij_equation = []
        for j in range(number_of_elements):
            mu_ij_equation.append(mu_ij[i][j] / math.cos(psi))

        # sum of all variables by coeficients from previous two sums
        concentrations = np.sum([mu_1j_equation, mu_ij_equation], axis=0)
        concentrations = np.multiply(concentrations, c1)
        concentrations = np.append(concentrations, c2)
        concentrations_equations.append(concentrations)

    # nsolve(equations, predicted_values) - druhy parameter nsolve su predikovane hodnoty, 
    # je potrebne volit parametre co najpresnejsie,
    # aby algoritmus nenasiel iba lokalne minimum ale globalne minimum
    variables = symbols('a0:%d, GdN1'%len(concentrations_equations), real=True)
    equations = quantitative_analysis_equation(variables, concentrations_equations)
    amounts = nsolve(equations, variables, [1] * (number_of_elements) + [100000000])
    amounts = np.array(amounts.tolist()).astype(np.float64)

    result = []
    if amounts is not None:
        for index in range(len(elements_energies)):
            result.append({"energy": elements_energies[index], "amount": amounts[index][0]})

    return json.dumps({"data": result}, cls = scripts_helper.NumpyEncoder)


## Quantitative analysis
def quantitative_analysis_simple(auger_probabilities_file_name, elements_info_file_name, elements, lines, areas):
    file = open(auger_probabilities_file_name, "r")
    file_content = file.read()
    file.close()
    auger_probabilities = json.loads(file_content)

    file = open(elements_info_file_name, "r", encoding="utf8")
    file_content = file.read()
    file.close()
    elements_info = json.loads(file_content)

    #energy_example = [4.65, 3.68, 3.27, 2.96, 7.05, 5.95, 6.40, 5.42]
    #elements_example = [26, 24, 26, 24]
    #lines_example = ["KM3", "KM3", "KL3", "KL3"]
    #areas_example = [21857, 31963, 134409, 192577]

    #elements = elements_example
    #lines = lines_example
    #areas = areas_example

    result = []
    for element, line, area in zip(elements, lines, areas):
        result.append({"element":  element, "line": line, "area": area, "amount": 0, "overall_quantity": 0})
    
    # Calculate real amount of mass per element
    for element in result:
        for probability in auger_probabilities[element["element"] - 1]["probabilities"]:
            if probability["line"] == element["line"]:
                element["amount"] = element["area"] / (1 - probability["probability"])
                break

    # Sum of all mass amount of elements
    amount_all = 0
    for element in result:
        amount_all += element["amount"]

    # Calculate overall quantity in %
    for element in result:
        element["overall_quantity"] = round((element["amount"] / amount_all) * 100, 2)

    # Add element info to the matches
    for element in result:
        element["element_info"] = elements_info[element["element"] - 1]["name"]

    return json.dumps(result, cls = scripts_helper.NumpyEncoder)


## Load mca file into structured json format
def load_mca(full_file_name):
    file = open(full_file_name, "r")
    file_content = file.read().split('\n')
    file.close()

    result = {"spectrum": {}, "calibration_label": [], "calibration_channel": [], "roi_start": [], "roi_end": [], "data": [], "configuration": {}, "status": {}}

    current_part = ""
    for line in file_content:
        if line.startswith("<<") and line != "<<END>>":
            current_part = line
            pass
        elif line == "<<END>>":
            continue
        else:
            if current_part == "<<PMCA SPECTRUM>>":
                line_splited = line.split(" - ", 1)
                result["spectrum"][line_splited[0]] = line_splited[1]
            elif current_part == "<<CALIBRATION>>":
                if line != "LABEL - Channel":
                    line_splited = line.split(" ", 1)
                    result["calibration_label"].append(int(line_splited[0]))
                    result["calibration_channel"].append(float(line_splited[1]))
            elif current_part == "<<ROI>>":
                line_splited = line.split(" ", 1)
                result["roi_start"].append(int(line_splited[0]))
                result["roi_end"].append(int(line_splited[1]))
            elif current_part == "<<DATA>>":
                result["data"].append(int(line))
            elif current_part == "<<DPP CONFIGURATION>>":
                line_splited = line.split(": ", 1)
                result["configuration"][line_splited[0]] = line_splited[1]
            elif current_part == "<<DPP STATUS>>":
                line_splited = line.split(": ", 1)
                result["status"][line_splited[0]] = line_splited[1]

    return json.dumps(result, cls = scripts_helper.NumpyEncoder, ensure_ascii = False)


## Implementation of Zhang fit for Adaptive iteratively reweighted penalized least squares for baseline fitting. 
## Modified from Original implementation by Professor Zhimin Zhang at https://github.com/zmzhang/airPLS/
## lambda_: parameter that can be adjusted by user. The larger lambda is,  the smoother the resulting background
def background_removal_zhangfit(data, lam = 100, itermax = 15):
    baseObj = scripts_helper.BaselineRemoval(data)
    output = baseObj.ZhangFit(lambda_ = lam, itermax = itermax)
    curve = data - output
    return json.dumps({"data": curve}, cls = scripts_helper.NumpyEncoder)


## Implementation of Modified polyfit method from paper: Automated Method for Subtraction of Fluorescence from Biological Raman Spectra, 
## by Lieber & Mahadevan-Jansen (2003)
## degree: Polynomial degree, default is 2
## repitition: How many iterations to run. Default is 100
## gradient: Gradient for polynomial loss, default is 0.001. It measures incremental gain over each iteration. 
## If gain in any iteration is less than this, further improvement will stop
def background_removal_modpoly(data, degree = 2, repitition = 100, gradient = 0.001):
    baseObj = scripts_helper.BaselineRemoval(data)
    output = baseObj.ModPoly(degree = degree, repitition = repitition, gradient = gradient)
    curve = data - output
    return json.dumps({"data": curve}, cls = scripts_helper.NumpyEncoder)


## IModPoly from paper: Automated Autofluorescence Background Subtraction Algorithm for Biomedical Raman Spectroscopy, 
## by Zhao, Jianhua, Lui, Harvey, McLean, David I., Zeng, Haishan (2007)
## degree: Polynomial degree, default is 2        
## repitition: How many iterations to run. Default is 100
## gradient: Gradient for polynomial loss, default is 0.001. It measures incremental gain over each iteration. 
## If gain in any iteration is less than this, further improvement will stop
def background_removal_imodpoly(data, degree = 2, repitition = 100, gradient = 0.001):
    baseObj = scripts_helper.BaselineRemoval(data)
    output = baseObj.IModPoly(degree = degree, repitition = repitition, gradient = gradient)
    curve = data - output
    return json.dumps({"data": curve}, cls = scripts_helper.NumpyEncoder)


## The Saviztky-Golay filtering can be considered as a generalized moving average filter. 
## It performs a least squares fit of a small set of consecutive data points to a polynomial 
## and takes the central point of the fitted polynomial curve as output.
##
## modes = mirror, constant, nearest, interp, wrap
def savitzky_golay_filter(data, window_length = 11, polyorder = 0, deriv = 0, delta = 1.0, axis = -1, mode = "interp", cval = 0.0):
    filtered_data = savgol_filter(data, window_length, polyorder, deriv, delta, axis, mode, cval)
    return json.dumps({"data": filtered_data}, cls = scripts_helper.NumpyEncoder)


## Gaussian filter modifies the input signal by convolution with a Gaussian function.
## This transformation is also known as the Weierstrass transform.
def gaussian_filter(data, sigma = 1, axis = -1, order = 0, output = None, mode = "reflect", cval = 0.0, truncate = 4.0):
    data = gaussian_filter1d(data, sigma, axis, order, output, mode, cval, truncate)
    return json.dumps({"data": data}, cls = scripts_helper.NumpyEncoder)


## The moving average is the most common filter in DSP, mainly because it is the easiest digital
## filter to understand and use. In spite of its simplicity, the moving average filter is optimal for
## a common task: reducing random noise while retaining a sharp step response. This makes it the
## premier filter for time domain encoded signals. However, the moving average is the worst filter
## for frequency domain encoded signals, with little ability to separate one band of frequencies from
## another. Relatives of the moving average filter include the Gaussian, Blackman, and multiplepass moving average. These have slightly better performance in the frequency domain, at the
## expense of increased computation time. 
def moving_average_filter(data, width = 2):
    wight_vector = 1 / ((2 * width) + 1)
    output = []
    for n in range(len(data)):
        yn = 0
        for i in range(-width, width):
            if (n - i) >= 0 and (n - i) < len(data):
                yn += data[n - i]
        output.append(wight_vector * yn)
    return json.dumps({"data": output}, cls = scripts_helper.NumpyEncoder)


## This function takes a 1-D array and finds all local maxima by simple comparison of neighboring values. 
## Optionally, a subset of these peaks can be selected by specifying conditions for a peak’s properties.
def get_peaks(data, height = None, threshold = None, distance = None, prominence = None, width = None, wlen = None):
    peaks, properties = find_peaks(data, height, threshold, distance, prominence, width, wlen, rel_height = 0.5, plateau_size = None)
    left_bases = scripts_helper.get_key(properties, "left_bases")
    right_bases = scripts_helper.get_key(properties, "right_bases")
    return json.dumps({"peaks_indexes": peaks.tolist(), "left_bases_indexes": left_bases, "right_bases_indexes": right_bases}, cls = scripts_helper.NumpyEncoder)


## Calibrate data using linear regression function with predict
def calibration(data_for_predict, data_x, data_y):
    data_for_predict = np.asarray(data_for_predict, dtype = float)
    x = np.asarray(data_x, dtype = float)
    y = np.asarray(data_y, dtype = float)
    result = stats.linregress(x, y)
    predict_values = result.intercept + result.slope * data_for_predict
    return json.dumps({"data": predict_values.tolist(), "slope": result.slope, "intercept": result.intercept, "rvalue": result.rvalue, "pvalue": result.pvalue, "stderr": result.stderr, "intercept_stderr": result.intercept_stderr}, cls = scripts_helper.NumpyEncoder)


## Asymmetric Least Squares Smoothing
## by P. Eilers and H. Boelens in 2005
## https://stackoverflow.com/questions/29156532/python-baseline-correction-library
##
## p - parameter of asymmetric least squares for baseline correction
## lam - to tune the balance between the smoothness and fitness
## niter - number of iterations
def background_removal_alss(data, lam, p, niter = 10):
    L = len(data)
    D = sparse.diags([1, -2, 1],[0, -1, -2], shape=(L, L - 2))
    D = lam * D.dot(D.transpose())
    w = np.ones(L)
    W = sparse.spdiags(w, 0, L, L)
    for _ in range(niter):
        W.setdiag(w)
        Z = W + D
        z = spsolve(Z, w * data)
        w = p * (data > z) + (1 - p) * (data < z)
    return json.dumps({"data": z}, cls = scripts_helper.NumpyEncoder)


## Function for testing purpose.
def test(arg1, arg2 = None, arg3 = None, arg4 = None):
    return json.dumps({"test": "TEST CALLED arg1 = " + str(arg1)}, cls = scripts_helper.NumpyEncoder)


## Main function.
if __name__ == "__main__":
    write_file("Program started!")
    while True:
        user_input = input()
        user_input = user_input.replace("\"", "\\\"")
        #print(user_input, file = sys.stderr)
        write_file(user_input)

        parser = ArgumentParser(description = __copyright__)
        parser.add_argument("-q", "--quit", action = "store_true", help = "Quit infinity user input with argument.")
        parser.add_argument("-t", "--test", action = "store_true", help = "Argument for testing purpose.")
        parser.add_argument("-f", "--function", help = "Function name.")
        parser.add_argument("-p", "--parameters", nargs = "+", help = "Parameters for function argument.")

        try:
            args = parser.parse_args(split(user_input))
        except:
            print("Error: Wrong argument!", file=sys.stderr) 
            continue

        if args.quit:
            ## Quit argument
            break
        elif args.test:
            ## Argument for testing purpose.
            print(json.dumps({"test": "test"}))
        elif args.function:
            try:
                ## Number of agruments of function by given argument.
                arg_count = locals()[args.function].__code__.co_argcount
            except KeyError as e:
                print("Error: Function name not exist.", file=sys.stderr)
                continue
            if args.parameters:                
                try:
                    #parameters = json.loads(base64ToString(args.parameters))
                    parameters = json.loads(" ".join(args.parameters).replace("\\", "\\\\"))
                except:
                    print("Error: Parameters have to be in validate json format.", file=sys.stderr)
                    continue
                try:
                    ## argument packing/unpacking
                    print(locals()[args.function](**parameters))
                except TypeError as e:
                    print("Error: %s." % str(e), file=sys.stderr)
                    continue
                except Exception as e:
                    print("Error: %s." % str(e), file=sys.stderr)
                    continue
            else:
                try:
                    ## argument packing/unpacking
                    print(locals()[args.function]())
                except TypeError as e:
                    print("Error: Missing required arguments.", file=sys.stderr)
