program abacaba;
begin
   number := 100;
   writeln('Number is: ', number);
   
   iptr := @number;
   writeln('iptr points to a value: ', iptr^);
   
   iptr^ := 200;
   parray[i]^ := 1;
   f()[5].f[g(5)] := 0;
   Book1.journal^.name := 'No pasaran!';
   Book2.subject := 'Telecom Billing Tutorial';
   Book1.s^.a := @number.num.num;
   writeln('Number is: ', number);
   writeln('iptr     points to a value: ', iptr^);
end.
