/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp'),
    sass = require('gulp-sass'),
    cssmin = require('gulp-cssmin'),
    uglify = require('gulp-uglify'),
    pump = require('pump');

gulp.task('styles', function () {
    return gulp.src('./src-scss/style.scss')
      .pipe(sass().on('error', sass.logError))
      .pipe(cssmin())
      .pipe(gulp.dest('./Content'));
});


gulp.task('js', function (cb) {
    pump([
        gulp.src('./src-js/main.js'),
        uglify(),
        gulp.dest('./Scripts/Custom')
    ],
      cb
    );
});



gulp.task('default', function () {
    gulp.watch(['./src-scss/style.scss'], ['styles']);
    gulp.watch(['./src-js/main.js'], ['js']);
});