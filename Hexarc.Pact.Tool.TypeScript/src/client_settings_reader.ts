/// <reference path="../types/index.d.ts" />
import * as File from "./io/file";


export async function read(): Promise<Pact.Tool.ClientSettings[] | null> {
  if (!await File.exists("pact.json")) return null;

  const raw = await File.read("pact.json");
  const document = JSON.parse(raw);

  if (Array.isArray(document)) return document;
  else return [document];
}
