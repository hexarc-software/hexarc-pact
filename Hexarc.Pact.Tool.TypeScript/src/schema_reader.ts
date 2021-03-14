import fetch from "node-fetch";


export async function read(schemaUri: string, scopes: string[] | undefined) {
  const response = await fetch(buildUri(schemaUri, scopes));
  if (!response.ok) throw new Error("Couldn't read schema");
  return await response.json();
}

function buildUri(schemaUri: string, scopes: string[] | undefined) {
  return [schemaUri, "?", buildQueryString(scopes)].join("");
}

function buildQueryString(scopes: string[] | undefined) {
  const namingConvetionQuery = toNamingConvetionQuery(NamingConvention.CamelCase);
  const scopeQuery = scopes == null ? "" : toScopeQuery(scopes);
  return [namingConvetionQuery, scopeQuery]
    .filter(x => x != null && x.length > 0)
    .join("&");
}

function toNamingConvetionQuery(namingConvention: NamingConvention) {
  return `namingConvention=${namingConvention}`;
}

function toScopeQuery(scopes: string[]) {
  scopes.map(x => `scope={x}`).join("&");
}

const enum NamingConvention {
  CamelCase = "CamelCase"
}
