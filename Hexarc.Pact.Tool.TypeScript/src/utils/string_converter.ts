export function fromPascalToUnderscore(value: string): string {
  return value
    .replace(/\.?([A-Z]+)/g, (x, y) => "_" + y.toLowerCase())
    .replace(/^_/, "");
}

export function removeSuffix(value: string, suffix: string): string {
  if (value.endsWith(suffix)) {
    return value.slice(0, value.length - suffix.length);
  } else {
    return value;
  }
}

export function lowerFirstLetter(value: string): string {
  return `${value[0].toLowerCase()}${value.slice(1)}`;
}