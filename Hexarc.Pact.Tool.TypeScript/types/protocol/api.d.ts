/// <reference path="./types.d.ts" />
/// <reference path="./type_references.d.ts" />

declare namespace Pact.Protocol.Api {
  const enum HttpMethod {
    Get = "Get",
    Post = "Post"
  }

  interface MethodParameter {
    readonly type: TypeReferences.TypeReference;
    readonly name: string;
  }

  interface Method {
    readonly name: string;
    readonly path: string;
    readonly httpMethod: HttpMethod;
    readonly returnType: TypeReferences.TaskTypeReference;
    readonly parameters: MethodParameter[];
  }

  interface Controller {
    readonly namespace?: string;
    readonly name: string;
    readonly path: string;
    readonly methods: Method[];
  }

  interface Schema {
    readonly controllers: Controller[];
    readonly types: Types.Type[];
  }
}
