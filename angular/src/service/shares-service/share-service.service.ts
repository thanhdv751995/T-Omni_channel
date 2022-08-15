import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ShareServiceService {
  public REST_API_SERVER = environment.apis.default.url;
  constructor(
    private httpClient: HttpClient,
  ) {}
  public postHttpClient(url : string, data : any) {
    return this.httpClient
      .post<any>(url, data)
      .pipe(catchError(err => this.handleError(err)));
  }
  public returnHttpClientGet(url: string) {
    return this.httpClient
      .get<any>(url)
      .pipe(catchError((err) => this.handleError(err)));
  }
  public putHttpClient(url: string, data: any) {
    return this.httpClient
      .put(url, data)
      .pipe(catchError((err) => this.handleError(err)));
  }
  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.log('An error occurred:', error.error.message);
    } else {
      console.log('status: ', error.status, error.message, error);

      // if(error.status == 403) {
      //   localStorage.clear();
      //   this.router.navigate(['sign-in']);
      // }
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong.
      console.log('error', error.error);
      console.error(`Backend returned code ${error.status}, ` + `body was: ${error.error}`);
    }
    // Return an observable with a user-facing error message.
    // this.spinnerService.requestSpinner();
    return throwError(error.error);
  }
}
