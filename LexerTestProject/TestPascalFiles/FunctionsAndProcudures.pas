program Hello;
var a: integer;
procedure check(var a:integer);
begin
writeln('Hello');
end;
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

