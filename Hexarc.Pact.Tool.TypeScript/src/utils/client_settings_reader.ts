import * as File from "./file";
import type { ClientSettings } from "../types/tool";


export async function read(): Promise<ClientSettings[] | null> {
  if (!await File.exists("pact.json")) return null;

  const raw = await File.read("pact.json");
  const document = JSON.parse(raw);

  if (Array.isArray(document)) return document;
  else return [document];
}
