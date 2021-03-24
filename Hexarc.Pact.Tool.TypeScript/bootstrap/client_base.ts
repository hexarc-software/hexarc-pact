export abstract class ClientBase {
  private _path: string;
  public get path() { return this._path; }
  public set path(value: string) { this._path = value; }

  private _headers: Record<string, string> = {};
  public get headers() { return this._headers };
  public set headers(value: Record<string, string>) { this._headers = value; }

  public constructor(path: string, headers?: Record<string, string>) {
    this._path = path;
    this._headers = headers ? Object.assign(this._headers, headers) : this._headers;
  }
}