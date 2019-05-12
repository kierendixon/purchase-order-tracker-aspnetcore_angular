import { Component } from '@angular/core';
import { ComponentFixture } from '@angular/core/testing';

export abstract class BasePage<T> {
  constructor(protected fixture: ComponentFixture<T>) {}

  protected query<V>(selector: string): V {
    return this.fixture.nativeElement.querySelector(selector);
  }

  protected queryAll<V>(selector: string): V[] {
    return this.fixture.nativeElement.querySelectorAll(selector);
  }
}
