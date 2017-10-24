import { Injectable, Injector } from '@angular/core';
import { MatSnackBar } from '@angular/material';

@Injectable()
export class SnackbarService {
  private static snackBar: MatSnackBar;

  constructor(private injector: Injector) {
    if (SnackbarService.snackBar === undefined) {
      setTimeout(() => SnackbarService.snackBar = injector.get(MatSnackBar));
    }
  }

  public showMessage(message: string): void {
    SnackbarService.snackBar.open(message, 'Dismiss', {
      duration: 2000
    });
  }
}
