import { Component, OnInit } from '@angular/core';
import { SignalRService } from 'src/service/signalR/signal-r.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  ngOnInit(): void {
    this.signalRService.SetConnection();
  }
  constructor(private signalRService: SignalRService) {}
}
