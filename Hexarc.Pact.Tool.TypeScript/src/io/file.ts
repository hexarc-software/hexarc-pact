import * as fs from "fs";


export async function read(path: string): Promise<string> {
  return await fs.promises.readFile(path, "utf8");
}

export async function copy(src: string, target: string): Promise<void> {
  await fs.promises.copyFile(src, target);
}

export async function exists(path: string): Promise<boolean> {
  const stat = await fs.promises.lstat(path);
  return stat.isFile();
}

export async function create(path: string, keepOpen?: boolean): Promise<fs.promises.FileHandle | undefined> {
  const handle = await fs.promises.open(path, "w");
  if (keepOpen) return handle;
  await handle.close();
}

export async function writeString(path: string, data: string): Promise<void> {
  await fs.promises.writeFile(path, data);
}
