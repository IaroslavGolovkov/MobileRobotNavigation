% close all;
% clear all;
% clc;
width = 600;
height = 600;
net = 1;

X = 1:net:width;
Y = 1:net:height;
Z = zeros(size(Y, 2), size(X, 2));

centerX = 300;
centerY = 100;
Dist_To_Corner = zeros(4,1);
Dist_To_Corner(1) = sqrt((centerX-0)^2 + (centerY-0)^2);
Dist_To_Corner(2) = sqrt((centerX-width)^2 + (centerY-0)^2);
Dist_To_Corner(3) = sqrt((centerX-0)^2 + (centerY-height)^2);
Dist_To_Corner(4) = sqrt((centerX-width)^2 + (centerY-height)^2);
r = max(Dist_To_Corner)+1;
Zcircle = -1;
zLow = 0;
zHigh = 1;

num_of_obstacles = 5;
obstaclesX = randi([0 width],1,num_of_obstacles);
obstaclesY = randi([0 height],1,num_of_obstacles);
obstclesSize = randi([10 50],1,num_of_obstacles);
A = 1;
for x = 1:size(X, 2)
    for y = 1:size(Y, 2)
        Z(y,x) = sqrt((x*net-centerX)^2+(y*net-centerY)^2)/r;
        for i = 1:num_of_obstacles
            sigma = obstclesSize(i);            
            Z(y,x) = Z(y,x) + A * exp(-((((x*net-obstaclesX(i))^2)/(2*sigma^2))+(((y*net-obstaclesY(i))^2)/(2*sigma^2))));
        end
    end
end
figure;
subplot(1,2,1);
mesh(X, Y, Z);

[px,py] = gradient(Z);
px = px * -100;
py = py * -100;
subplot(1,2,2);
contour(X,Y,Z)
hold on
quiver(X,Y,px,py)
hold off
