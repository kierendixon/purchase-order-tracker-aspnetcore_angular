import { Inject, Injectable, InjectionToken } from '@angular/core';

// https://angular.io/guide/dependency-injection-in-action#supply-a-custom-provider-with-inject
export const BROWSER_LOCAL_STORAGE = new InjectionToken<Storage>('Browser Local Storage', {
  providedIn: 'root',
  factory: () => localStorage
});

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {
  constructor(@Inject(BROWSER_LOCAL_STORAGE) public storage: Storage) {}

  get(key: string): string {
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
