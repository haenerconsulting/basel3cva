% Cholesky fails, with non-positive eigenvalues..
m=[[1,2];[2,4]];
%%chol(m);
% how handle (slightly) negative eigenvalues
%m=[[1,2.01];[2.01,4]];
[o,lambda]=eig(m);
ls=diag(lambda);
posls=arrayfun(@(x) x*(x>0),ls);
poslambda=diag(posls,0);
newm=o*poslambda*o';
sqrtnew=o*diag(sqrt(posls),0)*o';
% renormalize eigenvalues such that trace remains same
renls=posls*sum(ls)/sum(posls);
renlambda=diag(renls,0);
renm=o*renlambda*o';
sqrtren=o*diag(sqrt(renls),0),o';


    