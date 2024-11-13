import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError } from 'rxjs';
import { BadRequest } from 'src/app/models/badRequest';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error) {
        switch (error.status) {
          case 400:
            const item = new BadRequest(error);
            if (item.validationErrors && item.validationErrors.length > 0) {
            }
            throw item;
          case 401:
            const unauthorizedError = new BadRequest(error);
            throw unauthorizedError;
          case 404:
            const notFoundError = new BadRequest(error);
            throw notFoundError;
          case 500:
            throw new BadRequest(error);
          default:
            const defaultError = new BadRequest(error);
            throw defaultError;
        }
      }
      throw error;
    })
  )
};
