import { AttachFn, WorldParameters, World, WorldConstructorArgs } from 'cucumber';
import { Stream } from 'stream';
import { CliArgs } from './cliargs';

export class CustomWorld implements World {
  public readonly parameters: WorldParameters;
  public readonly attach: AttachFn;

  public cliArgs: CliArgs;
  // public actor:Actor;
  // public page:Page;
  // public docker: Docker;
  // public stepData: StepData = {};

  // TODO why these aren't provided?

  // console.log(JSON.stringify(arguments));
  // throw new Error('kdixon');

  // constructor({ attach, parameters }: WorldConstructorArgs) {
  //   this.attach = attach;
  //   this.parameters = parameters;
  // }
}

declare module 'cucumber' {
  interface World {
    parameters: WorldParameters;
    cliArgs: CliArgs;
    // actor:Actor
    // page:Page;
    // docker:Docker;
    // stepData:StepData
    attach: AttachFn;
  }

  interface WorldConstructorArgs {
    attach: AttachFn;
    parameters: WorldParameters;
  }

  interface WorldParameters {
    [key: string]: string;
  }

  // https://github.com/cucumber/cucumber-js/blob/master/docs/support_files/attachments.md
  // https://github.com/hdorgeval/cucumber-ts-starter/blob/master/world/custom-world.ts
  type MediaType = 'text/plain' | 'image/png' | 'application/json';
  type AttachBuffer = (data: Buffer, mediaType: MediaType) => void;
  type AttachStream = (data: Stream, mediaType: MediaType) => void;
  type AttachText = (data: string) => void;
  type AttachStringifiedJson = (data: string, mediaType: 'application/json') => void;
  type AttachBase64EncodedPng = (data: string, mediaType: 'image/png') => void;

  type AttachFn = AttachBuffer | AttachStream | AttachBase64EncodedPng | AttachStringifiedJson | AttachText;

  // add missing Promise<void> return type
  function After(code: HookCode): void | Promise<void>;
  function After(options: HookOptions | string, code: HookCode): void | Promise<void>;
  function AfterAll(code: GlobalHookCode): void | Promise<void>;
  function AfterAll(options: HookOptions | string, code: HookCode): void | Promise<void>;
  function Before(code: HookCode): void | Promise<void>;
  function Before(options: HookOptions | string, code: HookCode): void | Promise<void>;
  function BeforeAll(code: GlobalHookCode): void | Promise<void>;
  function BeforeAll(options: HookOptions | string, code: HookCode): void | Promise<void>;
}
