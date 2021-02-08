class LocalStorageService {
  constructor(public storage: Storage) {}

  get(key: string): string|null {
    return this.storage.getItem(key);
  }

  set(key: string, value: string): void {
    this.storage.setItem(key, value);
  }

  remove(key: string): void {
    this.storage.removeItem(key);
  }

  clear(): void {
    this.storage.clear();
  }
}

export { LocalStorageService };
