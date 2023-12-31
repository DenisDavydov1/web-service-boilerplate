//----------------------
// <auto-generated>
//   Generated using the NSwag toolchain v13.20.0.0 (NJsonSchema v10.9.0.0 (Newtonsoft.Json v10.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

/* tslint:disable */
/* eslint-disable */
// ReSharper disable InconsistentNaming



export interface IChangeUserPasswordDTO {
  oldPassword: string;
  newPassword: string;
}

export interface IEnqueueJobDTO {
  jobName: string;
  payload?: any | null;
}

export interface IGetAccessTokenDTO {
  login: string;
  password: string;
}

export interface IIdDTO {
  id: string;
}

export interface IJwtTokensDTO {
  accessToken: string;
  refreshToken: string;
}

export enum KafkaMessageType {
  System_Tests_Log = "System_Tests_Log",
  System_Tests_WithPayload = "System_Tests_WithPayload",
  System_Tests_NoPayload = "System_Tests_NoPayload",
}

export interface IKafkaProduceMessageDTO {
  topic: string;
  partition?: number | null;
  type: KafkaMessageType;
  payload?: any | null;
}

export interface IRefreshAccessTokenDTO {
  refreshToken: string;
}

export interface IRegisterUserDTO {
  login: string;
  password: string;
  name?: string | null;
  email?: string | null;
  languageCode: string;
  securityQuestions: { [key: string]: string; };
}

export interface IStoredFileDTO {
  name: string;
  createdBy: string;
  createdAt: Date;
  updatedBy?: string | null;
  updatedAt?: Date | null;
  id: string;
}

export interface IUpdateStoredFileDTO {
  name: string;
}

export interface IUserDTO {
  login: string;
  name?: string | null;
  email?: string | null;
  languageCode: string;
  role: UserRole;
  deletedBy?: string | null;
  deletedAt?: Date | null;
  createdBy: string;
  createdAt: Date;
  updatedBy?: string | null;
  updatedAt?: Date | null;
  id: string;
}

export enum UserRole {
  Viewer = "Viewer",
  User = "User",
  Moderator = "Moderator",
  Admin = "Admin",
}

export interface FileParameter {
  data: any;
  fileName: string;
}