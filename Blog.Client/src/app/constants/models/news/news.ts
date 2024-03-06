import { IGetUserResponseModel } from "../user/user";

export interface IListNewsResponseModel{
    title: string,
    description: string,
    id: string,
    author: IGetUserResponseModel
}