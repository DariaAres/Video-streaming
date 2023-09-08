import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { TokenStorageService } from '../services/token-storage.service';

export const notLoggedInGuard: CanActivateFn = (route, state) => {
  let tokenStorage = inject(TokenStorageService);
  let router = inject(Router);

  let token = tokenStorage.getToken();
  if (token) {
    router.navigate(['/']);
  }

  return !token;
};
