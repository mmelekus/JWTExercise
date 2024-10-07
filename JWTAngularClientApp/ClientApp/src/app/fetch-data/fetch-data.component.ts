import { Component } from '@angular/core';
import { WeatherForecast } from '../weather-forecast';
import { WeatherService } from '../weather.service';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[] = [];
  public jwtCookie: any;

  constructor(private weatherService: WeatherService) { }

  ngOnInit(): void {
    this.weatherService.getWeather().subscribe(data => {
      this.forecasts = data.body ?? [];
    })
  }

  postJwtCookie() : void {
    this.weatherService.postJwtToken().subscribe(data => {
      this.jwtCookie = JSON.stringify(data);
    })
  }
}
