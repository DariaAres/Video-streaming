import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { TokenStorageService } from '../services/token-storage.service';

export const adminGuard: CanActivateFn = (route, state) => {
  let service = inject(TokenStorageService);
  let router = inject(Router);

  let user = service.getUser();
  if (!user) {
    router.navigate(['/']);
    return false;
  }

  let isAdmin = user.username === 'admin';

  if (!isAdmin) {
    router.navigate(['/']);
  }

  return isAdmin;
};
