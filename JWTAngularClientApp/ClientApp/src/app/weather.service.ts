import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable  } from 'rxjs';
import { WeatherForecast } from './weather-forecast';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class WeatherService {
  private apiUrl = 'https://localhost:7258/WeatherForecast'; // Adjust the URL as needed
  // private jwtTestToken: string = '';
  
  constructor(private http: HttpClient, private cookieService: CookieService) { }

  getWeather(): Observable<HttpResponse<WeatherForecast[]>> {
    let resp = this.http.get<WeatherForecast[]>(this.apiUrl, { observe: 'response', withCredentials: true });
    
    return resp;
  }

  postJwtToken(): Observable<any> {
    let headers = {
      'Content-Type': 'text/plain'
    };
    let body = { data: '' };

    return this.http.post<any>(this.apiUrl, body, { headers: headers, withCredentials: true });
  }
}
