import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable  } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';
import { WeatherForecast } from './weather-forecast';

@Injectable({
  providedIn: 'root'
})
export class WeatherService {
  private apiUrl = 'https://localhost:7258/WeatherForecast'; // Adjust the URL as needed

  constructor(private http: HttpClient, private cookieService: CookieService) { }

  getWeather(): Observable<WeatherForecast[]> {
    return this.http.get<WeatherForecast[]>(this.apiUrl);
  }

  postJwtToken(): Observable<any> {
    // let cookieValue = this.cookieService.get('JWTTestToken');
    let cookies = this.cookieService.getAll();
    let headers = {
      'Content-Type': 'text/plain'
    };
    let body = { data: '' };

    // this.cookieService.set('JWTTestToken',cookieValue);

    return this.http.post<any>(this.apiUrl, body, { headers: headers, withCredentials: true });
  }
}
