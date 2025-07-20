// src/app/app.component.ts
import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from './navbar/navbar.component';
import { RouterOutlet } from '@angular/router';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'], // or './app.component.css' if you prefer CSS
  standalone: true,
  imports: [RouterOutlet,NavbarComponent], // Import NavbarComponent to use it in this component
  providers: [] // Add any necessary providers here, such as services or guards
})
export class AppComponent implements OnInit {
  constructor() {
  }
  title = 'oauthdemo';
  
  ngOnInit(): void {
    console.log("app component is loaded")
  }
}