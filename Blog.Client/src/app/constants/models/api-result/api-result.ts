export interface IApiResult<TResultValue>{
    isSuccess: boolean,
    isFailure: boolean,
    error: IError,
    value: TResultValue
};

export interface IError{
    code: number,
    message: string
};