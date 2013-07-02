n = 1000;
mu = 1;
sigma = 20;
x = normrnd(mu,sigma,n,1);
hist(x);
start=[0,1];
% confidence is are lower/upper bounds of 95% confidence interval
[estimate,confidence] = mle(x, 'distribution','norm');
% general distributions
%[estimate,confidence] = mle(x, 'pdf',@(x,mu,sigma) normpdf(x,mu,sigma), 'start',start, 'lower',[-Inf,0])