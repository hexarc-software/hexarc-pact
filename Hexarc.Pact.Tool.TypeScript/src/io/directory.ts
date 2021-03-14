import * as fs from "fs";
import * as path from "path";


export async function exists(path: string): Promise<boolean> {
  const stat = await fs.promises.lstat(path);
  return stat.isDirectory();
}

export async function clear(dirPath: string): Promise<void> {
  const found = await exists(dirPath);
  if (!found) return;

  const items = await fs.promises.readdir(dirPath);

  for (const item of items) {
    const subPath = path.join(dirPath, item);
    const stat = await fs.promises.lstat(subPath);
    if (stat.isDirectory()) {
      await remove(subPath);
    } else {
      await fs.promises.unlink(subPath);
    }
  }
}

export async function remove(dirPath: string): Promise<void> {
  const found = await exists(dirPath);
  if (!found) return;

  const items = await fs.promises.readdir(dirPath);

  for (const item of items) {
    const subPath = path.join(dirPath, item);
    const stat = await fs.promises.lstat(subPath);
    if (stat.isDirectory()) {
      await remove(subPath);
    } else {
      await fs.promises.unlink(subPath);
    }
  }

  await fs.promises.rmdir(dirPath);
}

export async function create(path: string): Promise<void> {
  const found = await exists(path);
  if (found) return;

  await fs.promises.mkdir(path);
}
