#!/usr/bin/env python
## Helper scripts for chart and processing data.


import cProfile
import pstats
import base64
import numpy
import json
import scipy


## author: Md Azimul Haque
## source: https://github.com/StatguyUser/BaselineRemoval
class BaselineRemoval():
    '''input_array: A pandas dataframe column provided in input as dataframe['input_df_column']. It can also be a Python list object
    degree: Polynomial degree
    '''     
    def __init__(self,input_array):
        self.input_array=input_array

    def poly(self,input_array_for_poly,degree_for_poly):
        '''qr factorization of a matrix. q` is orthonormal and `r` is upper-triangular.
        - QR decomposition is equivalent to Gram Schmidt orthogonalization, which builds a sequence of orthogonal polynomials that approximate your function with minimal least-squares error
        - in the next step, discard the first column from above matrix.

        - for each value in the range of polynomial, starting from index 0 of pollynomial range, (for k in range(p+1))
            create an array in such a way that elements of array are (original_individual_value)^polynomial_index (x**k)
        - concatenate all of these arrays created through loop, as a master array. This is done through (numpy.vstack)
        - transpose the master array, so that its more like a tabular form(numpy.transpose)'''
        input_array_for_poly = numpy.array(input_array_for_poly)
        X = numpy.transpose(numpy.vstack((input_array_for_poly**k for k in range(degree_for_poly+1))))
        return numpy.linalg.qr(X)[0][:,1:]
    def ModPoly(self,degree=2,repitition=100,gradient=0.001):
        '''Implementation of Modified polyfit method from paper: Automated Method for Subtraction of Fluorescence from Biological Raman Spectra, by Lieber & Mahadevan-Jansen (2003)
        
        degree: Polynomial degree, default is 2
        repitition: How many iterations to run. Default is 100
        gradient: Gradient for polynomial loss, default is 0.001. It measures incremental gain over each iteration. If gain in any iteration is less than this, further improvement will stop
        '''

        #initial improvement criteria is set as positive infinity, to be replaced later on with actual value
        criteria=numpy.inf

        baseline=[]
        corrected=[]

        ywork=self.input_array
        yold=self.input_array
        yorig=self.input_array

        polx=self.poly(list(range(1,len(yorig)+1)),degree)
        nrep=0

        while (criteria>=gradient) and (nrep<=repitition):
            ypred=predict(linear_regression(polx,yold), polx)
            ywork=numpy.array(numpy.minimum(yorig,ypred))
            criteria=sum(numpy.abs((ywork-yold)/yold))
            yold=ywork
            nrep+=1
        corrected=yorig-ypred
        corrected=numpy.array(list(corrected))
        return corrected
    
    def IModPoly(self,degree=2,repitition=100,gradient=0.001):
        '''IModPoly from paper: Automated Autofluorescence Background Subtraction Algorithm for Biomedical Raman Spectroscopy, by Zhao, Jianhua, Lui, Harvey, McLean, David I., Zeng, Haishan (2007)

        degree: Polynomial degree, default is 2        
        repitition: How many iterations to run. Default is 100
        gradient: Gradient for polynomial loss, default is 0.001. It measures incremental gain over each iteration. If gain in any iteration is less than this, further improvement will stop
        '''

        yold=numpy.array(self.input_array)
        yorig=numpy.array(self.input_array)
        corrected=[]

        nrep=1
        ngradient=1

        polx=self.poly(list(range(1,len(yorig)+1)),degree)
        ypred=predict(linear_regression(polx,yold), polx)
        Previous_Dev=numpy.std(yorig-ypred)

        #iteration1
        yold=yold[yorig<=(ypred+Previous_Dev)]
        polx_updated=polx[yorig<=(ypred+Previous_Dev)]
        ypred=ypred[yorig<=(ypred+Previous_Dev)]
        regression=None

        for i in range(2,repitition+1):
            if i>2:
                Previous_Dev=DEV
                
            regression=linear_regression(polx_updated,yold)
            ypred=predict(regression, polx_updated)
            DEV=numpy.std(yold-ypred)

            if numpy.abs((DEV-Previous_Dev)/DEV) < gradient:
                break
            else:
                for i in range(len(yold)):
                    if yold[i]>=ypred[i]+DEV:
                        yold[i]=ypred[i]+DEV
        baseline=predict(regression, polx)
        corrected=yorig-baseline
        return corrected

    def _WhittakerSmooth(self,x,w,lambda_):
        '''
        Penalized least squares algorithm for background fitting

        input
            x: input data (i.e. chromatogram of spectrum)
            w: binary masks (value of the mask is zero if a point belongs to peaks and one otherwise)
            lambda_: parameter that can be adjusted by user. The larger lambda is,  the smoother the resulting background

        output
            the fitted background vector
        '''
        X=numpy.matrix(x)
        m=X.size
        i=numpy.arange(0,m)
        E=scipy.sparse.eye(m,format='csc')
        D=E[1:]-E[:-1] # numpy.diff() does not work with sparse matrix. This is a workaround.
        W=scipy.sparse.diags(w,0,shape=(m,m))
        A=scipy.sparse.csc_matrix(W+(lambda_*D.T*D))
        B=scipy.sparse.csc_matrix(W*X.T)
        background=scipy.sparse.linalg.spsolve(A,B)
        return numpy.array(background)

    def ZhangFit(self,lambda_=100, itermax=15):
        '''
        Implementation of Zhang fit for Adaptive iteratively reweighted penalized least squares for baseline fitting. Modified from Original implementation by Professor Zhimin Zhang at https://github.com/zmzhang/airPLS/
        
        lambda_: parameter that can be adjusted by user. The larger lambda is,  the smoother the resulting background
        '''

        yorig=numpy.array(self.input_array)
        corrected=[]

        m=yorig.shape[0]
        w=numpy.ones(m)
        for i in range(1,itermax+1):
            corrected=self._WhittakerSmooth(yorig,w,lambda_)
            d=yorig-corrected
            dssn=numpy.abs(d[d<0].sum())
            if(dssn<0.001*(abs(yorig)).sum() or i==itermax):
                break
            w[d>=0]=0 # d>0 means that this point is part of a peak, so its weight is set to 0 in order to ignore it
            w[d<0]=numpy.exp(i*numpy.abs(d[d<0])/dssn)
            w[0]=numpy.exp(i*(d[d<0]).max()/dssn) 
            w[-1]=w[0]
        return yorig-corrected


## author: moshevi
## source: https://stackoverflow.com/questions/582336/how-can-you-profile-a-python-script
class _ProfileFunc:
    def __init__(self, func, sort_stats_by):
        self.func =  func
        self.profile_runs = []
        self.sort_stats_by = sort_stats_by

    def __call__(self, *args, **kwargs):
        pr = cProfile.Profile()
        pr.enable()  # this is the profiling section
        retval = self.func(*args, **kwargs)
        pr.disable()

        self.profile_runs.append(pr)
        ps = pstats.Stats(*self.profile_runs).sort_stats(self.sort_stats_by)
        return retval, ps


## author: moshevi
## source: https://stackoverflow.com/questions/582336/how-can-you-profile-a-python-script
def cumulative_profiler(amount_of_times, sort_stats_by = "time"):
    def real_decorator(function):
        def wrapper(*args, **kwargs):
            nonlocal function, amount_of_times, sort_stats_by  # for python 2.x remove this row

            profiled_func = _ProfileFunc(function, sort_stats_by)
            for i in range(amount_of_times):
                retval, ps = profiled_func(*args, **kwargs)
            ps.print_stats()
            return retval  # returns the results of the function
        return wrapper

    if callable(amount_of_times):  # incase you don"t want to specify the amount of times
        func = amount_of_times  # amount_of_times is the function in here
        amount_of_times = 5  # the default amount
        return real_decorator(func)
    return real_decorator


## Safe check if input is convertable into float.
def is_float(input):
    try:
        float(input)
        return True
    except (ValueError, TypeError):
        return False


def linear_regression(data_x, data_y):
    x = numpy.asarray(data_x, dtype = float)
    y = numpy.asarray(data_y, dtype = float)
    return scipy.stats.linregress(x, y)


def predict(regression, data_for_predict):
    data_for_predict = numpy.asarray(data_for_predict, dtype = float)
    return regression.intercept + regression.slope * data_for_predict


## Safe check if input is convertable into int.
def is_int(input):
    try:
        int(input)
        return True
    except (ValueError, TypeError):
        return False


## Safe check if input is None.
def is_none(input):
    return True if input == "None" or input is None else False


## Safe check if input is list or array type.
def is_array(input):
    return isinstance(input, list)


## Safe convert input array of strings to array of int, otherwise None.
def to_array_int(input):
    try:
        output = list(map(int, input))
        return output
    except (ValueError, TypeError):
        return None


## Safe convert input array of strings to array of float, otherwise None.
def to_array_float(input):
    try:
        output = list(map(float, input))
        return output
    except (ValueError, TypeError):
        return None


## Safe convert input to int, otherwise None.
def to_int(input):
    try:
        output = int(input)
        return output
    except (ValueError, TypeError):
        return None


## Safe convert input to string, otherwise None.
def to_float(input):
    try:
        output = float(input)
        return output
    except (ValueError, TypeError):
        return None


## Safe convert input to string, otherwise None.
def to_string(input):
    try:
        output = str(input)
        return output
    except (ValueError, TypeError):
        return None


def string_to_base64(input):
    return base64.b64encode(input.encode("utf-8"))


def base64_to_string(input):
    return base64.b64decode(input).decode("utf-8")


def get_attribute(input, attribute):
    return input.__getattribute__(attribute) if hasattr(input, attribute) else None


def get_key(input, key):
    return input[key] if key in input else None


## Special json encoder for numpy types
class NumpyEncoder(json.JSONEncoder):
    def default(self, obj):
        if isinstance(obj, numpy.ndarray):
            return obj.tolist()
        return json.JSONEncoder.default(self, obj)