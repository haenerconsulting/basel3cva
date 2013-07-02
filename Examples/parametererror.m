Strike = 100;
AssetPrice = linspace(1,200);
Sigma = 0.4;
Rates = 0.04;
Settle = 'Mar-05-13';
Maturity = 'Mar-05-14';


RateSpec = intenvset('ValuationDate', Settle, 'StartDates', Settle, 'EndDates',...
Maturity, 'Rates', Rates, 'Compounding', -1);

DividendType = 'continuous';
DividendAmounts = 0.04;

StockSpec = stockspec(Sigma, AssetPrice, DividendType, DividendAmounts);

OptSpec = 'call';

OutSpec = {'Price', 'Vega'};
[Price,Vega] = optstocksensbybls(RateSpec, StockSpec, Settle, Maturity, OptSpec, Strike, 'OutSpec', OutSpec);
optstocksensbybls(RateSpec, StockSpec, Settle, Maturity, OptSpec, Strike, 'OutSpec', OutSpec);

figure();
plot(AssetPrice,Vega);
xlabel('S');
ylabel('vega');
%title_handle = title('Vega');

figure();
plot(AssetPrice,Price);
xlabel('S');
ylabel('p');

SigmaError=0.04;
Error=Vega*SigmaError;
RelativeError=Error ./ Price*100;

figure();
plot(AssetPrice, Error);
xlabel('S');
ylabel('Error');

figure();
plot(AssetPrice,RelativeError);
xlabel('S');
ylabel('Rel Error [%]');