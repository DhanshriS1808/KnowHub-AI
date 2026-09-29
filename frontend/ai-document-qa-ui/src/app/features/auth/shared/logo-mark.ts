import { Component, input } from '@angular/core';

/** The KnowHub layered-hexagon mark. */
@Component({
  selector: 'app-logo-mark',
  template: `
    <svg [attr.width]="size()" [attr.height]="size()" viewBox="0 0 48 48" aria-hidden="true">
      <defs>
        <linearGradient id="kh-hex" x1="0" y1="0" x2="1" y2="1">
          <stop offset="0" stop-color="#5b8cff" />
          <stop offset="1" stop-color="#9d6bff" />
        </linearGradient>
        <linearGradient id="kh-layer" x1="0" y1="0" x2="1" y2="1">
          <stop offset="0" stop-color="#ffffff" />
          <stop offset="1" stop-color="#dfe6ff" />
        </linearGradient>
      </defs>
      <path d="M24 2 43 13v22L24 46 5 35V13z" fill="url(#kh-hex)" />
      <path d="m24 27 12-6.5-12-6.5-12 6.5z" fill="url(#kh-layer)" />
      <path d="m12 26 12 6.5 12-6.5" fill="none" stroke="#fff" stroke-width="2.6" stroke-linejoin="round" opacity=".85" />
      <path d="m12 31.5 12 6.5 12-6.5" fill="none" stroke="#fff" stroke-width="2.6" stroke-linejoin="round" opacity=".6" />
    </svg>
  `,
  styles: `
    :host {
      display: inline-flex;
      flex: none;
      filter: drop-shadow(0 6px 14px rgba(110, 100, 255, 0.35));
    }
  `,
})
export class LogoMark {
  readonly size = input(40);
}
