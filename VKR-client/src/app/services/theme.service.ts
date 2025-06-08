import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {

  private darkMode: boolean = false;
  public changeColorScheme(theme: string): void {
    if (theme === 'light') this.darkMode = false;
    else this.darkMode = true;
    document.body.classList.toggle('dark-mode', this.darkMode);
    localStorage.setItem('darkMode', String(this.darkMode));
  }
  constructor() { }
}
