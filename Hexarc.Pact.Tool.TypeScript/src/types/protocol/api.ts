import type { Type } from "./types";
import type { TypeReference, TaskTypeReference } from "./type_references";


export const enum HttpMethod {
  Get = "Get",
  Post = "Post"
}

export interface MethodParameter {
  readonly type: TypeReference;
  readonly name: string;
}

export interface Method {
  readonly name: string;
  readonly path: string;
  readonly httpMethod: HttpMethod;
  readonly returnType: TaskTypeReference;
  readonly parameters: MethodParameter[];
}

export interface Controller {
  readonly namespace?: string;
  readonly name: string;
  readonly path: string;
  readonly methods: Method[];
}

export interface Schema {
  readonly controllers: Controller[];
  readonly types: Type[];
}
