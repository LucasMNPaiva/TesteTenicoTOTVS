import { HttpInterceptorFn } from "@angular/common/http";
import { environment } from "../../../environments/environment";

export const apiUrlInterceptor: HttpInterceptorFn = (req, next) => {
  // Só prefixa se a URL começar com "/" (rota relativa)
  if (req.url.startsWith('/')) {
    const cloned = req.clone({ url: `${environment.apiUrl}${req.url}` });
    return next(cloned);
  }
  return next(req);
};