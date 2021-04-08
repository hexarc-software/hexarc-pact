export class HttpError<T extends any = any> extends Error {
  private _statusCode: number;
  public get statusCode() { return this._statusCode; }

  private _body?: T;
  public get body() { return this._body; }

  public constructor(statusCode: number, body?: T) {
    super();
    this._statusCode = statusCode;
    this._body = body;
  }
}