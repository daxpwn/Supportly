import { Component } from '@angular/core';

/** "About What We Do & Who We Are" section. */
@Component({
  selector: 'app-about',
  templateUrl: './about.html',
})
export class AboutComponent {
  readonly boxes: readonly string[] = [
    'Maintance Problems',
    '24/7 Support & Help',
    'Fixing Issues About',
    'Co. Development',
  ];
}
