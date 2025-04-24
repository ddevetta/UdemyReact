select * from ddv.place order by id;

select * from ddv.image order by id;

select * from ddv.userplace order by userid, placeid;

select * from ddv.places order by id;

select * from ddv.userplaces order by userid, id;

select * from ddv."__EFMigrationsHistory";

insert into ddv.place values('p1','Forest Waterfall',44.5588,-80.344);

insert into ddv.image values('p1','forest-waterfall.jpg','A tranquil forest with a cascading waterfall amidst greenery.');

delete from ddv.place where id = 'p111';
delete from ddv.image where id = 'p111';