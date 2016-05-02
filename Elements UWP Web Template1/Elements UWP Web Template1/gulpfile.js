/// <binding ProjectOpened='default' />
// Load the required modules
var gulp = require('gulp'),
    debug = require('gulp-debug'),
    plumber = require('gulp-plumber'),
    bower = require('gulp-bower'),
    main_bower_files = require('main-bower-files'),
    concat = require('gulp-concat'),
    uglify = require('gulp-uglify'),
    sass = require('gulp-sass'),
    cssmin = require('gulp-cssmin'),
    filter = require('gulp-filter'),
    del = require('del'),
    config = require('./project.config.json');

// Function to filter the files based on extension
var filterByExtension = function (extension) {
    return filter(function (file) {
        return file.path.match(new RegExp('.' + extension + '$'));
    });
};

var errorHandler = function (error) {
    console.log(error);
    this.emit('end');
}

var resolveMinifiedPath = function (path) {
    var params = path.split("/");
    var file = params.splice(params.length - 1, 1)[0];
    var newPath = params.join("/") + "/";

    return {
        file: file,
        path: newPath
    };
}

// #region minifcation

// Clean the distributable css directory
gulp.task('minify:clean:css', function (done) {
    del(config.files.css.dest, done);
});

// Clean the distributable js directory
gulp.task('minify:clean:js', function (done) {
    del(config.files.js.dest, done);
});

// Compile out sass files and minify it
gulp.task('minify:css', ['minify:clean:css'], function () {

    var min = resolveMinifiedPath(config.files.css.min);

    return gulp.src(config.files.css.src)
        .pipe(plumber(errorHandler))
        .pipe(sass())
        .pipe(gulp.dest(config.files.css.dest))
        .pipe(cssmin())
        .pipe(concat(min.file))
        .pipe(gulp.dest(min.path));
});

// Minify and obsfucate for produdction javascript code
gulp.task('minify:js', [], function () {

    var min = resolveMinifiedPath(config.files.js.min);

    return gulp.src(config.files.js.dest)
        .pipe(plumber(errorHandler))
        .pipe(uglify())
        .pipe(concat(min.file))
        .pipe(gulp.dest(min.path));
});

// Watch files for changes and repeat respective operations
gulp.task('minify:watch', function () {
    gulp.watch(config.files.css[config.files.css.watch], ['minify:css']);
    gulp.watch(config.files.js[config.files.js.watch], ['minify:js']);
});

// #endregion

// #region bower

// Clean the libraries directory
gulp.task('bower:clean', function (done) {
    del([config.files.lib], done);
});

// Install the libraries to the bower_components directory
gulp.task('bower:install', ['bower:clean'], function () {

    var js = filterByExtension('js');
    var css = filterByExtension('css');
    var fonts = filterByExtension('ttf');

    return gulp.src(main_bower_files())
        .pipe(debug())
        .pipe(plumber(errorHandler))
        .pipe(js)
        .pipe(gulp.dest(config.files.js.lib))
        .pipe(js.restore())
        .pipe(css)
        .pipe(gulp.dest(config.files.css.lib))
        .pipe(css.restore())
        .pipe(fonts)
        .pipe(gulp.dest(config.files.fonts.lib));
});

// #endregion

// Executable tasks
gulp.task('install', ['bower:install', 'bower:clean'], function () { });
gulp.task('default', ['minify:css', 'minify:js', 'minify:watch'], function () { });