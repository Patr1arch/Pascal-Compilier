

procedure check(var



function heh(var b:integer) : integer;
begin
writeln('World');
heh := b  + 1;
end;
begin
  a := 5;
  check(a);
  a := heh(a);
  writeln (a);
end.