export class FakeBrowserStorage implements Storage {
  private storage = {};

  [name: string]: any;
  length: number;

  public clear(): void {
    this.storage = {};
  }

  getItem(key: string): string {
    return this.storage[key];
  }

  key(index: number): string {
    const keys = Object.keys(this.storage);

    return index >= keys.length ? null : this.storage[keys[index]];
  }

  removeItem(key: string): void {
    delete this.storage[key];
  }

  setItem(key: string, value: string): void {
    this.storage[key] = value;
  }
}
