

import { Pipe, PipeTransform } from '@angular/core';
import { MarkRequestState } from './models';

@Pipe({
  name: 'callback',
  pure: false
})
export class CallbackPipe implements PipeTransform {
  transform(items: any[], callback: (item: any) => boolean): any {
    if (!items || !callback) {
      return items;
    }
    return items.filter(item => callback(item));
  }
}
