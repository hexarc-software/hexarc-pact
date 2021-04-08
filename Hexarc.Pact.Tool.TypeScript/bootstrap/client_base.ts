import { HttpError } from "./http_error";

export abstract class ClientBase {
  private _path: string;
  public get path() { return this._path; }
  public set path(value: string) { this._path = value; }

  private _headers: Record<string, string> = {};
  public get headers() { return this._headers };
  public set headers(value: Record<string, string>) { this._headers = value; }

  private _onError: (error: HttpError) => void = () => { };
  public get onError() { return this._onError; }
  public set onError(value: (error: HttpError) => void) { this._onError = value; }

  public constructor(path: string, headers?: Record<string, string>, onError?: (error: HttpError) => void) {
    this._path = path;
    this._headers = headers ? Object.assign(this._headers, headers) : this._headers;
    this._onError = onError ?? this._onError;
  }
}