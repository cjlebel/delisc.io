const gulp = require("gulp"),
    concat = require("gulp-concat"),
    sass = require("gulp-sass")(require("sass")),
    sourcemaps = require("gulp-sourcemaps"),
    autoprefixer = require("gulp-autoprefixer"),
    rename = require("gulp-rename");
(minifycss = require("gulp-minify-css")),
    (fileinclude = require("gulp-file-include")),
    (livereload = require("gulp-livereload")),
    (connect = require("gulp-connect")),
    (imagemin = require("gulp-imagemin")),
    (open = require("gulp-open")),
    (lr = require("tiny-lr"));

server = lr();

gulp.task("html", async function () {
    gulp.src(["./src/pages/**/*.html"])
        .pipe(
            fileinclude({
                prefix: "##",
                basepath: "@file",
            })
        )
        .pipe(gulp.dest("./dist"));
});

// // Replace placeholders with actual content
// gulp.task("process-templates", function () {
//     return gulp
//         .src("dist/temp.html")
//         .pipe(
//             replace(
//                 "<!--HEADER-->",
//                 gulp
//                     .src("src/partials/header.html")
//                     .pipe(concat(""))
//                     .pipe(rename(""))
//             )
//         )
//         .pipe(
//             replace(
//                 "<!--BREADCRUMBS-->",
//                 gulp
//                     .src("src/partials/breadcrumbs.html")
//                     .pipe(concat(""))
//                     .pipe(rename(""))
//             )
//         )
//         .pipe(
//             replace(
//                 "<!--FOOTER-->",
//                 gulp
//                     .src("src/partials/footer.html")
//                     .pipe(concat(""))
//                     .pipe(rename(""))
//             )
//         )
//         .pipe(gulp.dest("dist/"));
// });

gulp.task("scss", async function () {
    gulp.src("./src/assets/scss/**/*.scss")
        .pipe(sass({ style: "compressed" }))
        .pipe(sourcemaps.init())
        .pipe(
            autoprefixer(
                "last 2 version",
                "safari 5",
                "ie 8",
                "ie 9",
                "opera 12.1",
                "ios 6",
                "android 4"
            )
        )
        .pipe(gulp.dest("./dist/assets/css"))
        .pipe(rename({ suffix: ".min" }))
        .pipe(minifycss())
        .pipe(sourcemaps.write("."))
        .pipe(livereload(server))
        .pipe(gulp.dest("./dist/assets/css"));
    // .pipe(notify({ message: 'SASS Task Complete!' }))
});

gulp.task("open-server", function () {
    var server = connect.server({
        name: "Deliscio-server",
        root: ["./dist/"],
        port: 4000,
        open: true,
        livereload: true,
    });
    return gulp.src("./").pipe(
        open({
            uri: "http://" + server.host + ":" + server.port,
        })
    );
});

gulp.task("close-server", function () {
    connect.server({
        port: server.port,
    });
    connect.serverClose();
});

gulp.task("imgs", function () {
    return gulp
        .src("./src/assets/imgs/**/*.*")
        .pipe(imagemin())
        .pipe(gulp.dest("./dist/assets/imgs"));
});

// Just move this folders
gulp.task("icons", function () {
    return gulp
        .src("./src/assets/icons/**/*")
        .pipe(gulp.dest("./dist/assets/icons"));
});

// Move js without compile or minify [Just move]
gulp.task("js", function () {
    return gulp.src("./src/assets/js/**/*").pipe(gulp.dest("./dist/assets/js"));
});

gulp.task("modules", function () {
    return gulp
        .src("./src/assets/modules/**/*")
        .pipe(gulp.dest("./dist/assets/modules"));
});

gulp.task("watch", function (event) {
    gulp.watch("./src/pages/**/*.html", gulp.series("html"));
    gulp.watch("./src/partials/**/*.html", gulp.series("html"));
    gulp.watch(
        ["./src/assets/scss/**/*.css", "./src/assets/scss/**/*.scss"],
        gulp.series("scss")
    );
    gulp.watch("./src/assets/img/**/*", gulp.series("imgs"));
    gulp.watch("./src/assets/icons/**/*", gulp.series("icons"));
    gulp.watch("./src/assets/js/**/*", gulp.series("js"));
    gulp.watch("./src/assets/modules/**/*", gulp.series("modules"));
    livereload.listen();
    return;
});
