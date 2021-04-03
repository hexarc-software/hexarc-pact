
import { ClientBase } from "./client_base";


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

  protected async getJson<TResponse>(path: string, args?: GetMethodArgument[]): Promise<TResponse> {
    const pairs = args != null ? args.filter(x => x.value != null).map(x => `${x.name}=${x.value}`).join("&") : "";
    const query = !!pairs ? `?${pairs}` : "";
    const url = `${this.client.path}${this._path}${path}${query}`;
    const headers = Object.assign(this.client.headers, { "Content-Type": "application/json" });
    const response = await fetch(url, { method: "GET", headers });
    if (!response.ok) throw new Error(`Request filed with status ${response.statusText}`);
    return await response.json();
  }

  protected async postJson<TRequest, TResponse>(path: string, request: TRequest): Promise<TResponse> {
    const url = `${this.client.path}${this._path}${path}`;
    const body = JSON.stringify(request);
    const headers = Object.assign(this.client.headers, { "Content-Type": "application/json" });
    const response = await fetch(url, { method: "POST", body, headers });
    if (!response.ok) throw new Error(`Request filed with status ${response.statusText}`)
    return await response.json();
  }
}

export interface GetMethodArgument {
  name: string;
  value: any | undefined | null;
}