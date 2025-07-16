import { Component } from '@angular/core';
import { MatToolbar } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-navbar',
  imports: [MatButtonModule, MatToolbar, RouterLink, RouterLinkActive],
  standalone: true,
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {
  public onLogin(): void {
    console.log("test login")
    
  }
  public onLogout(): void {
    console.log("test logout")
  }
}
