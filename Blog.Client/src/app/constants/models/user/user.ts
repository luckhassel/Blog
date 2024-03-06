export interface ICreateUserModel{
    firstName: string,
    lastName: string,
    email: string,
    password: string
};

export interface ILoginUserModel{
    email: string,
    password: string
};

export interface ILoginUserResultModel{
    token: string
}

export interface IGetUserResponseModel{
    firstName: string,
    lastName: string,
    email: string,
    id: string
}