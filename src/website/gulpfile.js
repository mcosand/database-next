﻿/// <binding Clean='clean' />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify");

var paths = {
    webroot: "./wwwroot/"
};

paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "css/site.min.css";

gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("clean", ["clean:js", "clean:css"]);
/*
gulp.task("min:js", function () {
    return gulp.src([paths.js, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});
*/
gulp.task("css", function () {
    return gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});
/*
gulp.task("min", ["min:js", "min:css"]);
*/
gulp.task("js:lib", function (cb) {
  return gulp.src([
    paths.webroot + "lib/angular/angular.min.js",
    paths.webroot + "lib/angular-animate/angular-animate.min.js",
    paths.webroot + "lib/angular-aria/angular-aria.min.js",
    paths.webroot + "lib/angular-material/angular-material.min.js"
  ])
  .pipe(concat(paths.webroot + "js/lib.min.js"))
  .pipe(gulp.dest("."));
})

gulp.task("js:app", function (cb) {
  return gulp.src([
    paths.webroot + "js/site.js"
  ])
  .pipe(concat(paths.webroot + "js/site.min.js"))
  .pipe(uglify())
  .pipe(gulp.dest("."));
})
gulp.task("js", ["js:lib", "js:app"]);

gulp.task("default", ["js", "css"]);