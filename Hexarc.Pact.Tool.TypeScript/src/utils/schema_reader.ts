import fetch from "node-fetch";
import type { Schema } from "../types/protocol/api";


export async function read(schemaUri: string, scopes: string[] | undefined): Promise<Schema> {
  const response = await fetch(buildUri(schemaUri, scopes));
  if (!response.ok) throw new Error("Couldn't read schema");
  return await response.json() as Schema;
}

function buildUri(schemaUri: string, scopes: string[] | undefined) {
  return [schemaUri, "?", buildQueryString(scopes)].join("");
}

function buildQueryString(scopes: string[] | undefined) {
  const namingConventionQuery = toNamingConventionQuery(NamingConvention.CamelCase);
  const scopeQuery = scopes == null ? "" : toScopeQuery(scopes);
  return [namingConventionQuery, scopeQuery]
    .filter(x => x != null && x.length > 0)
    .join("&");
}

function toNamingConventionQuery(namingConvention: NamingConvention) {
  return `namingConvention=${namingConvention}`;
}

function toScopeQuery(scopes: string[]) {
  return scopes.map(x => `scope=${x}`).join("&");
}

const enum NamingConvention {
  CamelCase = "CamelCase"
}
