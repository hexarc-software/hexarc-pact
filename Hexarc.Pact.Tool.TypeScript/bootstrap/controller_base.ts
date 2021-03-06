
import { ClientBase } from "./client_base";
import { HttpError } from "./http_error";


export abstract class ControllerBase {
  private _client: ClientBase;
  public get client() { return this._client; }

  private _path: string;
  public get path() { return this._path; }
  public set path(value: string) { this._path = value; }

  public constructor(client: ClientBase, path: string) {
    this._client = client;
    this._path = path;
  }

  protected async _doGetRequestWithVoidResponse(path: string, args?: GetMethodArgument[]): Promise<void> {
    const url = this._computeEndpointUrl(path, args);
    const headers = Object.assign(this.client.headers, { "Content-Type": "application/json" });
    const response = await fetch(url, { method: "GET", headers });
    await this._processVoidResponse(response);
  }

  protected async _doGetRequestWithJsonResponse<TResponse>(path: string, args?: GetMethodArgument[]): Promise<TResponse> {
    const url = this._computeEndpointUrl(path, args);
    const headers = Object.assign(this.client.headers, { "Content-Type": "application/json" });
    const response = await fetch(url, { method: "GET", headers });
    return await this._processJsonResponse(response);
  }

  protected async _doPostVoidRequestWithVoidResponse(path: string): Promise<void> {
    const url = this._computeEndpointUrl(path);
    const headers = Object.assign(this.client.headers, { "Content-Type": "application/json" });
    const response = await fetch(url, { method: "POST", headers });
    await this._processVoidResponse(response);
  }

  protected async _doPostVoidRequestWithJsonResponse<TResponse>(path: string): Promise<TResponse> {
    const url = this._computeEndpointUrl(path);
    const headers = Object.assign(this.client.headers, { "Content-Type": "application/json" });
    const response = await fetch(url, { method: "POST", headers });
    return await this._processJsonResponse(response);
  }

  protected async _doPostJsonRequestWithVoidResponse<TRequest>(path: string, request: TRequest): Promise<void> {
    const url = this._computeEndpointUrl(path);
    const body = JSON.stringify(request);
    const headers = Object.assign(this.client.headers, { "Content-Type": "application/json" });
    const response = await fetch(url, { method: "POST", body, headers });
    await this._processVoidResponse(response);
  }

  protected async _doPostJsonRequestWithJsonResponse<TRequest, TResponse>(path: string, request: TRequest): Promise<TResponse> {
    const url = this._computeEndpointUrl(path);
    const body = JSON.stringify(request);
    const headers = Object.assign(this.client.headers, { "Content-Type": "application/json" });
    const response = await fetch(url, { method: "POST", body, headers });
    return await this._processJsonResponse(response);
  }

  private _computeEndpointUrl(path: string, args?: GetMethodArgument[]): string {
    const pairs = args != null ? args.filter(x => x.value != null).map(x => `${x.name}=${x.value}`).join("&") : "";
    const query = !!pairs ? `?${pairs}` : "";
    return `${this.client.path}${this._path}${path}${query}`;
  }

  private async _processJsonResponse<TResponse>(response: Response): Promise<TResponse> {
    if (response.ok) return await response.json();
    const httpError = await HttpErrorUtils.extractHttpError(response);
    this.client.onError(httpError);
    throw httpError;
  }

  private async _processVoidResponse(response: Response): Promise<void> {
    if (response.ok) return;
    const httpError = await HttpErrorUtils.extractHttpError(response);
    this.client.onError(httpError);
    throw httpError;
  }
}

module HttpErrorUtils {

  export async function extractHttpError(response: Response): Promise<HttpError> {
    const code = response.status;
    const body = await tryReadBody(response);
    return new HttpError(code, body);
  }

  async function tryReadBody(response: Response): Promise<any> {
    return await tryReadJson(response) ?? await tryReadText(response);
  }

  async function tryReadJson(response: Response): Promise<any> {
    try {
      return await response.json();
    } catch {
      return undefined;
    }
  }

  async function tryReadText(response: Response): Promise<any> {
    try {
      return await response.text();
    } catch {
      return undefined;
    }
  }

}

export interface GetMethodArgument {
  name: string;
  value: any | undefined | null;
}
