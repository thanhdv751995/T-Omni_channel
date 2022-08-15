import { Pipe, PipeTransform } from "@angular/core";

@Pipe({name: 'formatDescription'})
export class FormatDescription implements PipeTransform {
  transform(value: string): string {
      var vl = value.replace(/<(.|\n)*?>/g, '').trim();
      return vl.replace(/\&nbsp;/g, ' ')
   }
}